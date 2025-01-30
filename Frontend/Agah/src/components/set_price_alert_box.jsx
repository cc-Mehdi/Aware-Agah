import { React, useState, useEffect, useRef } from 'react'
import Dropdown from './dropdown'
import NumericalInput from './numerical_input'
import { sendPriceAlert } from './../services/api_SetPriceAlert';
import { getProductNames } from './../services/api_GetProductNames';

const set_price_alert_box = () => {

    const [productList, setProductList] = useState([]);
    const isMounted = useRef(false); // for fixing bug (this component send 2 request when loaded and this code fix this send just 1 request)


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
                createdAt: "2025-01-29T19:11:58.386Z"
            });

            console.log('API Response:', response);
        } catch (error) {
            console.error('API Error:', error);
        }
    };

    return (
        <>
            <div className="w-full p-6 py-18 flex flex-col justify-center items-center bg-white border border-gray-200 rounded-lg shadow-sm dark:bg-gray-800 dark:border-gray-700">

                <div className='w-full my-2'>
                    <Dropdown inputName={"Product"} label={"محصول را انتخاب کنید"} dataSource={productList} />
                </div>
                <div className='w-full flex my-2'>
                    <NumericalInput inputName={"MinPrice"} label={"حداقل"} />
                    <NumericalInput inputName={"MaxPrice"} label={"حداکثر"} />
                </div>

                <button onClick={handleSendAlert} type="button" className="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm w-full sm:w-auto px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">Submit</button>
            </div>

        </>
    )
}

export default set_price_alert_box