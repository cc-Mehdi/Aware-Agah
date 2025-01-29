import React from 'react'

const product_status_card = ({ name, amount, changedPercent, changedAmount, unit }) => {

    // Format amount with commas
    const formattedAmount = amount.toLocaleString();

    return (

        <div class="max-w-sm p-6 bg-white border border-gray-200 rounded-lg shadow-sm dark:bg-gray-800 dark:border-gray-700">
            <i class="fa-solid fa-medal text-3xl text-gray-500 dark:text-gray-400 mb-3"></i>
            <h5 class="mb-2 text-lg font-semibold tracking-tight text-gray-900 dark:text-white">{name}</h5>
            <p class="mb-3 font-normal text-gray-500 dark:text-gray-400">قیمت : <span className='font-bold text-xl text-gray-200'>{formattedAmount}</span> <span>{unit}</span></p>
            <span href="#" class="inline-flex font-medium items-center text-green-300">
                <span className='mx-1'>({changedPercent})</span>
                <span>{changedAmount}</span>
            </span>
        </div>

    )
}

export default product_status_card