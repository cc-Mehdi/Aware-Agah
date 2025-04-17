import React, { useState, useEffect, useRef } from "react";
import { useNavigate } from "react-router-dom";
import Logo from "./../assets/images/logos/Full color.png";
import { getUser } from '../services/api_BaseAPICaller';

const Navbar = () => {
    const navigate = useNavigate();
    const [isDropdownOpen, setIsDropdownOpen] = useState(false);
    const dropdownRef = useRef(null);
    const buttonRef = useRef(null);
    const [user, setUser] = useState(false);

    const handleLogout = () => {
        localStorage.removeItem("token");
        navigate("/login");
    };

    // Toggle dropdown visibility
    const toggleDropdown = () => {
        setIsDropdownOpen((prev) => !prev);
    };

    // Close dropdown when clicking outside
    useEffect(() => {
        const fetchUser = async () => {
            try {
                const response = await getUser();
                setUser(response || []);
            } catch (error) {
                dispatch(setError(error.message)); // ارسال خطا به Redux
                setUser("no_data");
            }
        };

        fetchUser();

        const handleClickOutside = (event) => {
            if (
                dropdownRef.current &&
                !dropdownRef.current.contains(event.target) &&
                buttonRef.current &&
                !buttonRef.current.contains(event.target)
            ) {
                setIsDropdownOpen(false);
            }
        };

        document.addEventListener("mousedown", handleClickOutside);
        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
        };
    }, []);

    useEffect(() => {
        if (isDropdownOpen && dropdownRef.current && buttonRef.current) {
            const dropdownRect = dropdownRef.current.getBoundingClientRect();
        }
    }, [isDropdownOpen]);

    return (
        <nav className="bg-white border-gray-200 mb-5 px-6 rounded-b-lg border shadow-sm dark:bg-gray-800 dark:border-gray-700">
            <div className="max-w-screen-xl flex flex-wrap items-center justify-between mx-auto p-4">
                {/* Logo */}
                <a href="/" className="flex items-center space-x-3 rtl:space-x-reverse">
                    <img src={Logo} className="h-8" alt="Logo" />
                    <span className="self-center text-2xl font-semibold whitespace-nowrap dark:text-white ms-2">
                        اطلاع رسانی آگاه
                    </span>
                </a>

                {/* User Dropdown */}
                <div className="relative">
                    {/* Avatar Button */}
                    <img
                        ref={buttonRef}
                        id="avatarButton"
                        type="button"
                        className="w-10 h-10 bg-white rounded-full cursor-pointer shadow-lg"
                        src="https://cdn0.iconfinder.com/data/icons/social-messaging-ui-color-shapes/128/user-male-circle-blue-512.png"
                        alt="User dropdown"
                        onClick={toggleDropdown}
                    />

                    {/* Dropdown Menu */}
                    {isDropdownOpen && (
                        <div
                            ref={dropdownRef}
                            id="userDropdown"
                            className={`absolute left-0 z-10 bg-white divide-y divide-gray-100 rounded-lg shadow-sm max-w-[90vw] min-w-max dark:bg-gray-700 dark:divide-gray-600 mt-2`}
                        >
                            <div className="px-4 py-3 text-sm text-gray-900 dark:text-white">
                                <div>{user.fullname ? user.fullname : 'کاربر آگاه'}</div>
                                <div className="font-medium truncate">{user.email}</div>
                            </div>
                            <ul className="py-2 text-sm text-gray-700 dark:text-gray-200">
                                <li>
                                    <button onClick={() => navigate("/Profile")} className="w-full cursor-pointer block px-4 py-2 hover:bg-gray-100 dark:hover:bg-gray-600 dark:hover:text-white">
                                        مدیریت حساب
                                    </button>
                                </li>
                            </ul>
                            <div className="py-1">
                                <button
                                    onClick={handleLogout}
                                    className="cursor-pointer block py-2 px-3 text-white bg-rose-700 rounded-sm w-full text-center dark:text-white"
                                >
                                    خروج
                                </button>
                            </div>
                        </div>
                    )}
                </div>
            </div>
        </nav>
    );
};

export default Navbar;
