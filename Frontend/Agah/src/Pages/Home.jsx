import React from 'react'
import ProductStatusList from './../components/product_status_card_list'
import SetPriceRangeReservation from '../components/setPriceRangeReservation_section'

const Home = () => {
    return (
        <>
            <ProductStatusList />
            <SetPriceRangeReservation />
        </>
    )
}

export default Home