import { React, useState, useEffect, useRef } from 'react'
import { getReserve } from '../../services/api_GetReserve';
import { useDispatch } from "react-redux";
import { formatNumber, convertToPersianDate } from '../../utility/helper';

const profile_active_reserves = () => {
    const [reserve, setReserve] = useState({
        product: {
            persianName: '',
        },
        minPrice: 0,
        maxPrice: 0,
    }); const dispatch = useDispatch();
    const isMounted = useRef(false); // for fixing bug (this component send 2 request when loaded and this code fix this send just 1 request)

    useEffect(() => {
        const fetchUser = async () => {
            try {
                const response = await getReserve();
                setReserve(response || []);
            } catch (error) {
                dispatch(setError(error.message)); // ارسال خطا به Redux
                setReserve("no_data");
            }
        };

        // Fetch api's only once
        if (!isMounted.current) {
            fetchUser();
            isMounted.current = true;
        }
    });

    return (
        <>
            <div className="bg-white border-gray-200 my-5 px-6 py-6 rounded-lg border shadow-sm dark:bg-gray-800 dark:border-gray-700">
                <i className="fa-solid fa-bolt me-2 text-white text-lg" aria-hidden="true"></i>
                <span className='text-white font-bold text-3xl'>رزرو فعال</span>
                <div className="max-w-sm mx-auto my-10">

                    <div className="w-full p-4 text-center bg-white border border-gray-200 rounded-lg shadow-sm sm:p-8 dark:bg-gray-800 dark:border-gray-700">
                        <h5 className="mb-2 text-3xl font-bold text-gray-900 dark:text-white">{reserve.product.persianName}</h5>
                        <p className="mb-5 text-md text-gray-500 sm:text-lg dark:text-gray-400">زمان رزرو محصول :</p>
                        <p className="mb-5 text-md text-gray-500 sm:text-lg dark:text-gray-400">{convertToPersianDate(reserve.createdAt, "HH:mm:ss")} | {convertToPersianDate(reserve.createdAt)}</p>
                        <div className="items-center justify-center space-y-4 sm:flex sm:space-y-0 sm:space-x-4 rtl:space-x-reverse">
                            <div className="mx-1 w-full sm:w-auto bg-gray-800 hover:bg-gray-700 focus:ring-4 focus:outline-none focus:ring-gray-300 text-white rounded-lg inline-flex items-center justify-center px-4 py-2.5 dark:bg-gray-700 dark:hover:bg-gray-600 dark:focus:ring-gray-700">
                                <i className="fa-solid fa-arrow-up me-2 text-white text-lg" aria-hidden="true"></i>
                                <div className="text-left rtl:text-right">
                                    <div className="mb-1 text-xs">حداکثر قیمت</div>
                                    <div className="-mt-1 font-sans text-sm font-semibold">{formatNumber(reserve.minPrice)}</div>
                                </div>
                            </div>
                            <div className="mx-1 w-full sm:w-auto bg-gray-800 hover:bg-gray-700 focus:ring-4 focus:outline-none focus:ring-gray-300 text-white rounded-lg inline-flex items-center justify-center px-4 py-2.5 dark:bg-gray-700 dark:hover:bg-gray-600 dark:focus:ring-gray-700">
                                <i className="fa-solid fa-arrow-down me-2 text-white text-lg" aria-hidden="true"></i>
                                <div className="text-left rtl:text-right">
                                    <div className="mb-1 text-xs">حداقل قیمت</div>
                                    <div className="-mt-1 font-sans text-sm font-semibold">{formatNumber(reserve.maxPrice)}</div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </>
    )
}

export default profile_active_reserves