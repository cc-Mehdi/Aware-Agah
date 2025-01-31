import { React, useState, useEffect, useRef } from 'react'
import Dropdown from './dropdown'
import NumericalInput from './numerical_input'
import PlatformSelector from './radiobutton_platform_selector'
import { sendPriceAlert } from './../services/api_SetPriceAlert';
import { getProductNames } from './../services/api_GetProductNames';

const set_price_alert_box = () => {

    const [productList, setProductList] = useState([]);
    const isMounted = useRef(false); // for fixing bug (this component send 2 request when loaded and this code fix this send just 1 request)
    const [selectedPlatform, setSelectedPlatform] = useState();


    // Fetch product names when the component mounts
    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const response = await getProductNames();

                // Ensure we correctly parse the inner JSON
                const parsedData = JSON.parse(response.result);

                setProductList(parsedData);
            } catch (error) {
                console.error("Failed to fetch product names:", error);
                setProductList(["خطا در دریافت داده‌ها"]);
            }
        };
        if (!isMounted.current) {
            fetchProducts(); // Fetch only once
            isMounted.current = true;
        }

    }, []);

    const handleSendAlert = async () => {
        try {
            const response = await sendPriceAlert({
                userId: 5,
                product: document.querySelector("#Product").value,
                minPrice: document.querySelector("#MinPrice").value,
                maxPrice: document.querySelector("#MaxPrice").value,
                platform: selectedPlatform,
                createdAt: "2025-01-29T19:11:58.386Z"
            });

            console.log('API Response:', response);
        } catch (error) {
            console.error('API Error:', error);
        }
    };

    return (
        <>
            <div className="w-full p-6 py-14 flex flex-col justify-center items-center bg-white border border-gray-200 rounded-lg shadow-sm dark:bg-gray-800 dark:border-gray-700">

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
                    <PlatformSelector inputName={'Platform'} dataSource={
                        [{ 'id': '1', 'title': 'ایمیل', 'description': 'اطلاع رسانی با ایمیل' },
                        { 'id': '2', 'title': 'تلفن', 'description': 'اطلاع رسانی با تماس تلفنی' },
                        { 'id': '3', 'title': 'پیامک', 'description': 'اطلاع رسانی با پیامک' },
                        { 'id': '4', 'title': 'نوتیف درون برنامه', 'description': 'اطلاع رسانی با نوتیفیکیشن' }]
                    }
                        selectedValue={selectedPlatform}
                        onChange={setSelectedPlatform} />
                </div>

                <button onClick={handleSendAlert} type="button" className="text-white w-full py-5 mt-7 hover:bg-gray-800 cursor-pointer bg-white border border-gray-200 rounded-lg shadow-sm dark:bg-gray-600 dark:border-gray-700">ثبت</button>
            </div>

        </>
    )
}

export default set_price_alert_box