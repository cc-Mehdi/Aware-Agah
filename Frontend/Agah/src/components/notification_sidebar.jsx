import React, { useState, useRef, useEffect } from "react";
import { readAllNotifications } from './../services/api_ReadAllNotifications';
import NotificationCard from './notification_card';

const NotificationSidebar = () => {
    const [isOpen, setIsOpen] = useState(false);
    const [notReadedNotificationsCount, setnotReadedNotificationsCount] = useState(0);
    const isMounted = useRef(false); // for fixing bug (this component send 2 request when loaded and this code fix this send just 1 request)
    const [notifications, setNotifications] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const response = await GetNotifications({ userId: 2, alarmEnglishName: "Alert" }); // TODO: Dynamic UserID
                setnotReadedNotificationsCount(response.filter(notification => !notification.isRead).length);
                setNotifications(response);

            } catch (error) {
                console.error('Error fetching gold prices:', error);
            }
        };


        // Fetch only once
        if (!isMounted.current) {
            fetchData();
            isMounted.current = true;
        }

        const interval = setInterval(() => {
            fetchData();
        }, 60000);

        return () => clearInterval(interval);
    }, []);

    const ReadAllNotifications_handler = async () => {
        const response = await readAllNotifications({ userId: 2 }); // TODO: Dynamic UserID
        setnotReadedNotificationsCount(response.filter(notification => !notification.isRead).length);
        setNotifications(response);
    }

    return (
        <>
            {/* Toggle Button (Fixed at Bottom Right) */}
            <button
                onClick={() => setIsOpen(!isOpen)}
                className="fixed bottom-5 right-5 z-50 p-4 cursor-pointer text-white bg-gray-600 rounded-full shadow-lg hover:bg-gray-700 focus:outline-none"
            >
                <i className="fa-solid fa-comments-dollar"></i>

                {/* Badge (only shown if count > 0) */}
                {notReadedNotificationsCount > 0 && (
                    <span className="absolute -top-1 -right-1 flex items-center justify-center w-5 h-5 text-xs font-bold text-white bg-red-500 rounded-full">
                        {notReadedNotificationsCount}
                    </span>
                )}
                {notReadedNotificationsCount > 9 && (
                    <span className="absolute -top-1 -right-1 flex items-center justify-center w-5 h-5 text-xs font-bold text-white bg-red-500 rounded-full">
                        <i className="fa-solid fa-infinity"></i>
                    </span>
                )}
            </button>

            {/* Sidebar */}
            <aside
                className={`fixed top-0 right-0 z-40 w-100 h-screen bg-gray-50 dark:bg-gray-800 shadow-lg transform transition-transform duration-300 ${isOpen ? "translate-x-0" : "translate-x-full"
                    }`}
            >
                <div className="h-full px-3 py-4 overflow-y-auto">
                    <button onClick={ReadAllNotifications_handler} type="button" className="text-white w-full py-2 hover:bg-gray-800 cursor-pointer bg-white border border-gray-200 rounded-lg shadow-sm dark:bg-gray-600 dark:border-gray-700">
                        <i className="fa-solid fa-check-double me-2"></i>
                        <span>
                            خواندن همه
                        </span>
                    </button>

                    {/* Sidebar Content */}
                    <ul className="space-y-2 font-medium mt-8">
                        {
                            notifications.map(notification => (
                                < li key={notification.id} >
                                    <NotificationCard notificationItem={notification} />
                                </li>
                            ))
                        }
                    </ul>
                </div>
            </aside >
        </>
    );
};

export default NotificationSidebar;
