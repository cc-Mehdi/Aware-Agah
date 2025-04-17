import { React, useState, useEffect, useRef } from 'react';
import { getUser, putUser } from '../../services/api_BaseAPICaller';
import { useDispatch } from 'react-redux';
import Toastr from '../../components/toastr';

const profile_setting_editUser = () => {
    const [user, setUser] = useState({});
    const dispatch = useDispatch();
    const isMounted = useRef(false); // for fixing bug (this component sends 2 requests when loaded, and this code fixes it to send just 1 request)
    const [toastr, setToastr] = useState(null); // State to manage Toastr message

    // Fetch user data on component mount
    useEffect(() => {
        const fetchUser = async () => {
            try {
                const response = await getUser();
                setUser(response || {});
            } catch (error) {
                dispatch(setError(error.message)); // Send error to Redux
                setUser('no_data');
            }
        };

        // Fetch API only once
        if (!isMounted.current) {
            fetchUser();
            isMounted.current = true;
        }
    }, [dispatch]);

    // Update user handler
    const UpdateUserHandler = async () => {
        if (!user.fullname || user.fullname.trim() === '') {
            setToastr({ type: 'error', title: 'اطلاعات را با دقت وارد کنید!' });
        } else {
            try {
                const response = await getUser(); // Fetch updated user data
                setUser(response || {});
                setToastr({ type: 'success', title: 'اطلاعات با موفقیت به‌روزرسانی شد!' });
            } catch (error) {
                dispatch(setError(error.message)); // Send error to Redux
                setToastr({ type: 'error', title: 'خطا در به‌روزرسانی اطلاعات!' });
            }
        }
    };

    // Clear Toastr after 3 seconds
    useEffect(() => {
        if (toastr) {
            const timer = setTimeout(() => setToastr(null), 3000);
            return () => clearTimeout(timer);
        }
    }, [toastr]);

    return (
        <>
            <div className="bg-white border-gray-200 my-5 px-6 py-6 rounded-lg border shadow-sm dark:bg-gray-800 dark:border-gray-700">

                {/* Conditionally render Toastr */}
                {toastr && <Toastr toastrType={toastr.type} title={toastr.title} onClose={() => setToastr(null)} />}

                <i className="fa-solid fa-gear me-2 text-white text-lg" aria-hidden="true"></i>
                <span className="text-white font-bold text-3xl">تنظیمات حساب</span>
                <div className="max-w-sm mx-auto my-10">
                    <div className="mb-5">
                        <label htmlFor="fullname" className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">
                            نام کامل
                        </label>
                        <input
                            onChange={(e) => setUser({ ...user, fullname: e.target.value })}
                            value={user.fullname || ''}
                            type="text"
                            id="fullname"
                            className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                            placeholder="نام کامل"
                            required
                        />
                    </div>
                    <button
                        onClick={UpdateUserHandler}
                        type="button"
                        className="text-white w-full py-2 hover:bg-gray-800 cursor-pointer bg-white border border-blue-200 rounded-lg shadow-sm dark:bg-blue-600 dark:border-blue-700"
                    >
                        <i className="fa-solid fa-pen me-2 text-xs" aria-hidden="true"></i>
                        <span>ویرایش</span>
                    </button>
                </div>
            </div>
        </>
    );
};

export default profile_setting_editUser;