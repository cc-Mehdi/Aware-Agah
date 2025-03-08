import { React, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Navbar from './../components/navbar';
import ProductStatusList from './../components/product_status_card_list';
import SetPriceRangeReservation from './../components/setPriceRangeReservation_section';
import NotificationSidebar from './../components/notification_sidebar';
import { validateToken } from './../services/api_ValidateToken';

const Home = () => {
    const navigate = useNavigate();
    const [isAuthenticated, setIsAuthenticated] = useState(null); // `null` means "still checking"

    useEffect(() => {
        const checkAuth = async () => {
            const isValid = await validateToken();
            if (!isValid) {
                localStorage.removeItem("token");
                navigate("/login");
            } else {
                setIsAuthenticated(true);
            }
        };

        checkAuth();
    }, []);

    if (isAuthenticated === null) {
        <div className="flex items-center justify-center h-screen text-white font-bolder text-3xl">در حال احراز هویت شما ...</div>
    }

    return (
        <>
            <Navbar />
            <NotificationSidebar />
            <ProductStatusList />
            <SetPriceRangeReservation />
        </>
    );
};

export default Home;
