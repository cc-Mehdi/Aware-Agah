import { React, useState, useEffect, useRef } from 'react';
import { api_SendEmailVerification, api_VerifyEmail, getUser } from '../../services/api_BaseAPICaller';
import { useDispatch } from 'react-redux';
import Toastr from '../../components/toastr';

const ProfileSettingActivation = () => {
    const [user, setUser] = useState({});
    const dispatch = useDispatch();
    const isMounted = useRef(false); // for fixing bug (this component sends 2 requests when loaded, and this code fixes it to send just 1 request)
    const [toastr, setToastr] = useState(null); // State to manage Toastr message
    const [activeTab, setActiveTab] = useState('email'); // 'email' or 'phone'
    const [token, setToken] = useState('');
    const [isLoading, setIsLoading] = useState(false);
    const [isEmailVerified, setIsEmailVerified] = useState(false);

    // Fetch user data on component mount
    useEffect(() => {
        const fetchUser = async () => {
            try {
                const response = await getUser();
                setUser(response || {});
                setIsEmailVerified(response.isEmailVerified);
            } catch (error) {
                dispatch(setError(error.message)); // Send error to Redux
                setUser({});
            }
        };

        // Fetch API only once
        if (!isMounted.current) {
            fetchUser();
            isMounted.current = true;
        }
    }, [dispatch]);

    const handleActivation = async (type) => {
        setIsLoading(true);
        try {
            const response = await api_VerifyEmail(token);
            if (response.statusCode === 200) {
                setToastr({
                    type: 'success',
                    title: response.statusMessage
                });
                setIsEmailVerified(true);
            }
            else {
                setToastr({
                    type: 'error',
                    title: response.statusMessage
                });
            }

        } catch (error) {
            setToastr({
                type: 'error',
                title: `خطا در تایید ${type === 'email' ? 'ایمیل' : 'شماره تلفن'}!خطا : ${error}`
            });
        } finally {
            setIsLoading(false);
        }
    };

    const resendCode = async (type) => {
        setIsLoading(true);
        try {
            const response = await api_SendEmailVerification();
            if (response.statusCode === 200) {
                setToastr({
                    type: 'success',
                    title: response.statusMessage
                });

            }
            else {
                setToastr({
                    type: 'error',
                    title: response.statusMessage
                });
            }

        } catch (error) {
            setToastr({
                type: 'error',
                title: `خطا در تایید ${type === 'email' ? 'ایمیل' : 'شماره تلفن'}!\nخطا : ${error}`
            });
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        if (toastr) {
            const timer = setTimeout(() => setToastr(null), 3000);
            return () => clearTimeout(timer);
        }
    }, [toastr]);

    return (
        <div className="bg-white border-gray-200 my-5 px-6 py-6 rounded-lg border shadow-sm dark:bg-gray-800 dark:border-gray-700">
            {toastr && <Toastr toastrType={toastr.type} title={toastr.title} onClose={() => setToastr(null)} />}

            <div className="flex border-b border-gray-200 dark:border-gray-700 mb-6">
                <button
                    className={`cursor-pointer py-2 px-4 font-medium ${activeTab === 'email' ? 'text-blue-600 border-b-2 border-blue-600' : 'text-gray-500'}`}
                    onClick={() => setActiveTab('email')}
                >
                    <i className="fa-solid fa-envelope me-2"></i>
                    فعال سازی ایمیل
                </button>
                <button
                    disabled
                    className={`cursor-not-allowed py-2 px-4 font-medium ${activeTab === 'phone' ? 'text-blue-600 border-b-2 border-blue-600' : 'text-gray-500'}`}
                    onClick={() => setActiveTab('phone')}
                >
                    <i className="fa-solid fa-phone me-2"></i>
                    فعال سازی شماره تلفن
                </button>
            </div>

            {activeTab === 'email' ? (
                <div className="max-w-sm mx-auto">
                    <div className="mb-5">
                        {isEmailVerified === true ? (
                            <>
                                <div className="mx-auto flex-col max-w-sm items-center gap-x-4 rounded-xl bg-white p-6 shadow-lg outline outline-green/5 dark:bg-green-800 dark:shadow-none dark:-outline-offset-1 dark:outline-white/10">
                                    <div className="text-xl font-medium text-black dark:text-white">
                                        <i className="fa-solid fa-check-circle text-white text-xl me-2"></i>
                                        <span>تایید ایمیل</span>
                                    </div>
                                    <p className="text-gray-500 dark:text-gray-400">
                                        ایمیل {user.email} با موفقیت تایید شده است!
                                    </p>
                                </div>
                            </>
                        ) : (
                            <>
                                <i className="fa-solid fa-envelope-circle-check text-yellow-500 text-xl"></i>
                                <div className='w-full'>
                                    <div className="text-xl font-medium text-black dark:text-white">تایید ایمیل</div>
                                    <p className="text-gray-500 dark:text-gray-400">
                                        لطفا ایمیل {user.email} را تایید کنید
                                    </p>
                                    <div className="mt-3 space-y-3 w-full">
                                        <input
                                            onChange={(e) => setToken(e.target.value)}
                                            type="text"
                                            placeholder="کد تایید"
                                            className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                        />
                                        <button
                                            onClick={() => handleActivation('email')}
                                            disabled={isLoading}
                                            className="cursor-pointer w-full py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50"
                                        >
                                            {isLoading ? 'در حال پردازش...' : 'تایید ایمیل'}
                                        </button>
                                        <button
                                            onClick={() => resendCode('email')}
                                            disabled={isLoading}
                                            className="cursor-pointer text-blue-600 hover:text-blue-800 dark:text-blue-400 dark:hover:text-blue-300 text-sm"
                                        >
                                            ارسال کد
                                        </button>
                                    </div>
                                </div>
                            </>
                        )}
                    </div>
                </div>
            ) : (
                <div className="max-w-sm mx-auto">
                    <div className="mb-5">
                        {user.isPhoneVerified ? (
                            <>
                                <i className="fa-solid fa-check-circle text-white text-xl"></i>
                                <div>
                                    <div className="text-xl font-medium text-black dark:text-white">تایید شماره تلفن</div>
                                    <p className="text-gray-500 dark:text-gray-400">
                                        شماره {user.phone} با موفقیت تایید شده است!
                                    </p>
                                </div>
                            </>
                        ) : (
                            <>
                                <i className="fa-solid fa-mobile-screen-button text-yellow-500 text-xl"></i>
                                <div className='w-full'>
                                    <div className="text-xl font-medium text-black dark:text-white">تایید شماره تلفن</div>
                                    <p className="text-gray-500 dark:text-gray-400">
                                        لطفا شماره تلفن {user.phone} را تایید کنید
                                    </p>
                                    <div className="mt-3 space-y-3 w-full">
                                        <input
                                            type="text"
                                            placeholder="کد تایید"
                                            className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                        />
                                        <button
                                            onClick={() => handleActivation('phone')}
                                            disabled={isLoading}
                                            className="cursor-pointer w-full py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:opacity-50"
                                        >
                                            {isLoading ? 'در حال پردازش...' : 'تایید شماره'}
                                        </button>
                                        <button
                                            onClick={() => resendCode('phone')}
                                            disabled={isLoading}
                                            className="cursor-pointer text-blue-600 hover:text-blue-800 dark:text-blue-400 dark:hover:text-blue-300 text-sm"
                                        >
                                            ارسال کد
                                        </button>
                                    </div>
                                </div>
                            </>
                        )}
                    </div>
                </div>
            )}
        </div>
    );
};

export default ProfileSettingActivation;