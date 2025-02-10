import React, { useEffect, useState, useRef } from 'react';
import ProductStatusCard from './product_status_card';
import { getProductsLog } from './../services/api_GetProductsLog';


const ProductStatusCardList = () => {
    const [goldData, setGoldData] = useState([]);
    const isMounted = useRef(false); // for fixing bug (this component send 2 request when loaded and this code fix this send just 1 request)

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await getProductsLog();
                setGoldData(response || []);
            } catch (error) {
                console.error('Error fetching gold prices:', error);
            }
        };

        if (!isMounted.current) {
            fetchData(); // Fetch only once
            isMounted.current = true;
        }

        const interval = setInterval(() => {
            fetchData();
        }, 5000);

        return () => clearInterval(interval);
    }, []);

    return (
        <>
            <div className='bg-white border-gray-200 my-5 px-6 py-14 rounded-lg border shadow-sm dark:bg-gray-800 dark:border-gray-700'>

                <div className='mb-10'>
                    <h5 className="text-gray-900 text-2xl font-semibold tracking-tight dark:text-white">قیمت محصولات</h5>
                    <span className="text-gray-500 text-lg font-bold">لیست قیمت محصولات مرتبط با طلا</span>
                </div>

                <div className="gap-6 grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3">
                    {goldData.length > 0 ? (
                        goldData.map((item, index) => (
                            <ProductStatusCard
                                key={index}
                                dataSourceItem={item}
                            />
                        ))
                    ) : (
                        <div role="status" class="max-w-sm animate-pulse">
                            <div class="h-2.5 bg-gray-200 rounded-full dark:bg-gray-700 w-48 mb-4"></div>
                            <div class="h-2 bg-gray-200 rounded-full dark:bg-gray-700 max-w-[360px] mb-2.5"></div>
                            <div class="h-2 bg-gray-200 rounded-full dark:bg-gray-700 mb-2.5"></div>
                            <div class="h-2 bg-gray-200 rounded-full dark:bg-gray-700 max-w-[330px] mb-2.5"></div>
                            <div class="h-2 bg-gray-200 rounded-full dark:bg-gray-700 max-w-[300px] mb-2.5"></div>
                            <div class="h-2 bg-gray-200 rounded-full dark:bg-gray-700 max-w-[360px]"></div>
                            <span class="sr-only">Loading...</span>
                        </div>)}
                </div>
            </div>
        </>
    );
};

export default ProductStatusCardList;
