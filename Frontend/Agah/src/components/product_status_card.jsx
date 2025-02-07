import React from 'react'

const product_status_card = ({ dataSourceItem }) => {


    return (

        <div className="max-w-sm p-6 bg-white border border-gray-200 rounded-lg shadow-sm dark:bg-gray-800 dark:border-gray-700">
            <i className={`${dataSourceItem.productIconName} text-3xl text-gray-500 dark:text-gray-400 mb-3`}></i>
            <h5 className="mb-2 text-lg font-semibold tracking-tight text-gray-900 dark:text-white">{dataSourceItem.productName}</h5>
            <p className="mb-3 font-normal text-gray-500 dark:text-gray-400">قیمت : <span className='font-bold text-xl text-gray-200'>{dataSourceItem.price}</span> <span>{dataSourceItem.unit}</span></p>
            <span href="#" className="inline-flex font-medium items-center text-green-300">
                <span className='mx-1'>({"0"})</span>
                <span>{"0"}</span>
            </span>
        </div>

    )
}

export default product_status_card