import React from 'react'
import ProductStatusList from './../components/product_status_card_list'
import SetPriceAlertBox from './../components/set_price_alert_box'

const Home = () => {
    return (
        <>
            <ProductStatusList />
            <SetPriceAlertBox />
        </>
    )
}

export default Home