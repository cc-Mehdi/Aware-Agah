import { React, useState, useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import { validateToken } from '../../services/api_ValidateToken';
import ProfileSettingEditUser from './profile_setting_editUser';
import ProfileSettingActivation from './profile_setting_activation';
import ProfileActiveReserve from './profile_active_reserves';
import Navbar from './../../components/navbar';


const Profile = () => {
    const isMounted = useRef(false); // for fixing bug (this component send 2 request when loaded and this code fix this send just 1 request)
    const navigate = useNavigate();
    const [isAuthenticated, setIsAuthenticated] = useState(null); // `null` means "still checking"
    const [activeComponent, setActiveComponent] = useState('settings'); // 'settings' or 'reserves'

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

        // Fetch api's only once
        if (!isMounted.current) {
            checkAuth();
            isMounted.current = true;
        }
    }, [navigate]);

    if (isAuthenticated === null) {
        return <div className="flex items-center justify-center h-screen text-white font-bolder text-3xl">در حال احراز هویت شما ...</div>;
    }

    return (
        <>
            <Navbar />
            <div className="w-full">
                <div className="bg-white border-gray-200 my-5 px-2 py-2 rounded-lg border shadow-sm dark:bg-gray-800 dark:border-gray-700">
                    <div className="flex">
                        <button
                            type="button"
                            className={`mx-1 text-white w-full py-2 hover:bg-gray-800 cursor-pointer bg-white border border-gray-200 rounded-lg shadow-sm dark:bg-gray-600 dark:border-gray-700 ${activeComponent === 'settings' ? 'bg-gray-800' : ''}`}
                            onClick={() => setActiveComponent('settings')}
                        >
                            <i className="fa-solid fa-gear me-2" aria-hidden="true"></i><span>تنظیمات حساب</span>
                        </button>
                        <button
                            type="button"
                            className={`mx-1 text-white w-full py-2 hover:bg-gray-800 cursor-pointer bg-white border border-gray-200 rounded-lg shadow-sm dark:bg-gray-600 dark:border-gray-700 ${activeComponent === 'reserves' ? 'bg-gray-800' : ''}`}
                            onClick={() => setActiveComponent('reserves')}
                        >
                            <i className="fa-solid fa-bolt me-2" aria-hidden="true"></i><span>رزور فعال</span>
                        </button>
                    </div>
                </div>

                {activeComponent === 'settings' && (
                    <>
                        <ProfileSettingEditUser />
                        <ProfileSettingActivation />
                    </>
                )}
                {activeComponent === 'reserves' && <ProfileActiveReserve />}
            </div>
        </>
    );
};

export default Profile;