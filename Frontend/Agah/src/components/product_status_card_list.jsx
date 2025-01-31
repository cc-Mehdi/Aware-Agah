import React, { useEffect, useState, useRef } from 'react';
import ProductStatusCard from './product_status_card';

const ProductStatusCardList = () => {
    const [goldData, setGoldData] = useState([]);
    const isMounted = useRef(false); // for fixing bug (this component send 2 request when loaded and this code fix this send just 1 request)

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await fetch('https://brsapi.ir/FreeTsetmcBourseApi/Api_Free_Gold_Currency.json');
                const data = await response.json();
                setGoldData(data.gold || []);
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
        }, 600000);

        return () => clearInterval(interval);
    }, []);

    return (
        <>
            <div className='bg-white border border-gray-200 rounded-lg shadow-sm dark:bg-gray-800 dark:border-gray-700 my-5 px-6 py-14'>

                <div className='mb-10'>
                    <h5 className="text-2xl font-semibold tracking-tight text-gray-900 dark:text-white">قیمت محصولات</h5>
                    <span className="font-bold text-lg text-gray-500">لیست قیمت محصولات مرتبط با طلا</span>
                </div>

                <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
                    {goldData.length > 0 ? (
                        goldData.map((item, index) => (
                            <ProductStatusCard
                                key={index}
                                name={item.name}
                                amount={item.price}
                                unit={item.unit}
                                changedPercent="0%"
                                changedAmount="0"
                            />
                        ))
                    ) : (
                        <p className="text-white">Loading...</p>
                    )}
                </div>
            </div>
        </>
    );
};

export default ProductStatusCardList;
