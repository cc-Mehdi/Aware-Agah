import { React, useState, useEffect, useRef } from 'react'
import Dropdown from './dropdown'
import NumericalInput from './numerical_input'
import PlatformSelector from './radiobutton_platform_selector'
import Toastr from './toastr';
import { useDispatch } from "react-redux";
import { setError } from "./../redux/errorSlice";
import { getAlarms, setPriceRangeReservation, getProductNames } from "./../services/api_BaseAPICaller"

const set_price_alert_box = () => {

    const dispatch = useDispatch();
    const [productList, setProductList] = useState([]);
    const [alarmList, setAlarmList] = useState([]);
    const isMounted = useRef(false); // for fixing bug (this component send 2 request when loaded and this code fix this send just 1 request)
    const [selectedPlatform, setSelectedPlatform] = useState();
    const [toastr, setToastr] = useState(null); // State to manage Toastr message


    useEffect(() => {
        const fetchProducts = async () => {     // Fetch product names when the component mounts
            try {
                const response = await getProductNames();
                setProductList(response || []);
            } catch (error) {
                dispatch(setError(error.message)); // ارسال خطا به Redux
                setProductList(["خطا در دریافت داده‌ها"]);
            }
        };

        const fetchAlarms = async () => {     // Fetch product names when the component mounts
            try {
                const response = await getAlarms();
                setAlarmList(response || []);
            } catch (error) {
                dispatch(setError(error.message)); // ارسال خطا به Redux
                setAlarmList("no_data");
            }
        };


        // Fetch api's only once
        if (!isMounted.current) {
            fetchProducts();
            fetchAlarms();
            isMounted.current = true;
        }

    }, []);

    const handleSendAlert = async () => {
        try {

            // validating form inputs
            let selectedProductId = document.querySelector("#Product").value;
            if (selectedProductId === "") {
                setToastr({ type: "warning", title: "لطفا محصول مورد نظر را انتخاب کنید" });
                document.querySelector("#Product").focus();
                return;
            }

            let selectedMinPrice = document.querySelector("#MinPrice").value;
            if (selectedMinPrice === "") {
                setToastr({ type: "warning", title: "لطفا حداقل قیمت مورد نظر را انتخاب کنید" });
                document.querySelector("#MinPrice").focus();
                return;
            }

            let selectedMaxPrice = document.querySelector("#MaxPrice").value;
            if (selectedMaxPrice === "") {
                setToastr({ type: "warning", title: "لطفا حداکثر قیمت مورد نظر را انتخاب کنید" });
                document.querySelector("#MaxPrice").focus();
                return;
            }

            if (selectedPlatform === undefined) {
                setToastr({ type: "warning", title: "لطفا نوع اعلان مورد نظر را انتخاب کنید" });
                return;
            }

            const response = await setPriceRangeReservation({
                alarmId: selectedPlatform,
                productId: selectedProductId,
                minPrice: selectedMinPrice,
                maxPrice: selectedMaxPrice,
            });

            if (response.statusCode === 200) {
                setToastr({ type: "success", title: response.statusMessage || "عملیات با موفقیت انجام شد" });
            }
            else
                setToastr({ type: "error", title: response.statusMessage || "عملیات با شکست مواجه شد" });

            setTimeout(() => setToastr(null), 5000);

        } catch (error) {
            dispatch(setError(error.message)); // ارسال خطا به Redux
        }
    };

    return (
        <>
            <div className="w-full p-6 py-14 flex flex-col justify-center items-center bg-white border border-gray-200 rounded-lg shadow-sm dark:bg-gray-800 dark:border-gray-700">

                {/* Conditionally render Toastr */}
                {toastr && <Toastr toastrType={toastr.type} title={toastr.title} />}

                <div className='w-full mb-10'>
                    <h5 className="mb-2 text-2xl font-semibold tracking-tight text-gray-900 dark:text-white">انتخاب بازه قیمت</h5>
                    <span className="font-bold text-lg text-gray-500">برای محصول مورد نظر بازه قیمتی مشخص کنید تا در صورت خارج شدن از بازه به شما هشدار داده شود</span>
                </div>

                <div className='w-full my-2'>
                    <Dropdown inputName={"Product"} label={"محصول را انتخاب کنید"} dataSource={productList} />
                </div>
                <div className='w-full flex my-2'>
                    <NumericalInput inputName={"MinPrice"} label={"حداقل"} />
                    <NumericalInput inputName={"MaxPrice"} label={"حداکثر"} />
                </div>
                <div className='w-full my-2'>
                    <PlatformSelector inputName={'Platform'} dataSource={alarmList}
                        selectedValue={selectedPlatform}
                        onChange={setSelectedPlatform} />
                </div>

                <button onClick={handleSendAlert} type="button" className="text-white w-full py-5 mt-7 hover:bg-gray-800 cursor-pointer bg-white border border-gray-200 rounded-lg shadow-sm dark:bg-gray-600 dark:border-gray-700">ثبت</button>
            </div>

        </>
    )
}

export default set_price_alert_box