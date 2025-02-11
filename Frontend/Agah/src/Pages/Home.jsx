import React from 'react'
import ProductStatusList from './../components/product_status_card_list'
import SetPriceRangeReservation from './../components/setPriceRangeReservation_section'
import NotificationSidebar from './../components/notification_sidebar'

const Home = () => {
    return (
        <>
            <NotificationSidebar />
            <ProductStatusList />
            <SetPriceRangeReservation />
        </>
    )
}

export default Home