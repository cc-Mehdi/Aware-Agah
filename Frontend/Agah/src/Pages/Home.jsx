import { React, useEffect } from 'react'
import ProductStatusList from './../components/product_status_card_list'
import SetPriceRangeReservation from './../components/setPriceRangeReservation_section'
import NotificationSidebar from './../components/notification_sidebar'
import { validateToken } from './../services/api_ValidateToken'
import { useNavigate } from 'react-router-dom'

const Home = () => {

    const navigate = useNavigate();

    useEffect(() => {
        validateToken().then((isValid) => {
            if (!isValid) {
                localStorage.removeItem("token");
                navigate("/login");
            }
        });
    }, []);

    return (
        <>
            <NotificationSidebar />
            <ProductStatusList />
            <SetPriceRangeReservation />
        </>
    )
}

export default Home