﻿import React, { useEffect, useState, useRef } from 'react';
import ProductStatusCard from './product_status_card';
import { getProductsLog } from './../services/api_BaseAPICaller';
import Skeleton from './skeleton';
import moment from "moment-jalaali";


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
                    <p className="mb-10 text-md text-gray-500 w-full text-start font-normal dark:text-gray-400">
                        <span className="text-gray-900 tracking-tight dark:text-gray-300">امروز : </span>
                        <span className="font-normal text-gray-700 dark:text-gray-400" dir="ltr">{moment(goldData.createdAt).format("jYYYY-jMM-jDD | HH:mm:ss")} {/* convert to persian date */}</span>
                    </p>
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
                        <Skeleton />
                    )}
                </div>
            </div>
        </>
    );
};

export default ProductStatusCardList;
