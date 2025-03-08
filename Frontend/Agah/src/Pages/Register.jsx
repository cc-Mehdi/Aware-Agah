import Logo from './../assets/images/logos/Full color.png';
import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { api_HandleRegister } from './../services/api_HandleRegister';
import Toastr from './../components/toastr';

const Register = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const navigate = useNavigate();
    const [toastr, setToastr] = useState(null); // State to manage Toastr message

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (email === "" || password === "" || confirmPassword === "") {
            setToastr({ type: "error", title: 'اطلاعات را با دقت وارد کنید!' });
        }
        else {
            if (password !== confirmPassword) {
                setToastr({ type: "error", title: 'رمزهای عبور مطابقت ندارند!' });
            }
            else {
                try {
                    const data = await api_HandleRegister(email, password);
                    if (data.statusCode === 200) {
                        setToastr({ type: "success", title: data.statusMessage || "عملیات با موفقیت انجام شد" });
                        navigate('/login');
                    }
                    else {
                        setToastr({ type: "error", title: data.statusMessage });
                    }
                } catch (error) {
                    setToastr({ type: "error", title: "عملیات با شکست مواجه شد\nخطا: " + error });
                }
            }
        }

        setTimeout(() => setToastr(null), 3000);

    };

    return (
        <>
            <div className="flex min-h-full w-full flex-1 flex-col justify-center px-6 py-12 lg:px-8 bg-gray-900">

                {/* Conditionally render Toastr */}
                {toastr && <Toastr toastrType={toastr.type} title={toastr.title} onClose={() => setToastr(null)} />}
                <div className="sm:mx-auto sm:w-full sm:max-w-sm">
                    <img alt="Agah" src={Logo} className="mx-auto h-18 rounded-xl w-auto" />
                    <h2 className="mt-10 text-center text-2xl/9 font-bold tracking-tight text-white">
                        ثبت نام در سامانه آگاه
                    </h2>
                </div>

                <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
                    <form className="space-y-6">
                        <div>
                            <label htmlFor="email" className="block text-sm/6 font-medium text-gray-300">
                                آدرس ایمیل
                            </label>
                            <div className="mt-2">
                                <input
                                    id="email"
                                    name="email"
                                    type="email"
                                    required
                                    autoComplete="email"
                                    className="block w-full rounded-md bg-gray-800 px-3 py-1.5 text-base text-white outline-1 -outline-offset-1 outline-gray-600 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-500 sm:text-sm/6"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                />
                            </div>
                        </div>

                        <div>
                            <label htmlFor="password" className="block text-sm/6 font-medium text-gray-300">
                                رمز عبور
                            </label>
                            <div className="mt-2">
                                <input
                                    id="password"
                                    name="password"
                                    type="password"
                                    required
                                    autoComplete="new-password"
                                    className="block w-full rounded-md bg-gray-800 px-3 py-1.5 text-base text-white outline-1 -outline-offset-1 outline-gray-600 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-500 sm:text-sm/6"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                />
                            </div>
                        </div>

                        <div>
                            <label htmlFor="confirm-password" className="block text-sm/6 font-medium text-gray-300">
                                تأیید رمز عبور
                            </label>
                            <div className="mt-2">
                                <input
                                    id="confirm-password"
                                    name="confirm-password"
                                    type="password"
                                    required
                                    autoComplete="new-password"
                                    className="block w-full rounded-md bg-gray-800 px-3 py-1.5 text-base text-white outline-1 -outline-offset-1 outline-gray-600 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-500 sm:text-sm/6"
                                    value={confirmPassword}
                                    onChange={(e) => setConfirmPassword(e.target.value)}
                                />
                            </div>
                        </div>

                        <div>
                            <button
                                onClick={handleSubmit}
                                type="submit"
                                className="cursor-pointer flex w-full justify-center rounded-md bg-indigo-500 px-3 py-1.5 text-sm/6 font-semibold text-white shadow-xs hover:bg-indigo-400 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-500"
                            >
                                ثبت نام
                            </button>
                        </div>
                    </form>

                    <p className="mt-10 text-center text-sm/6 text-gray-400">
                        قبلاً ثبت‌نام کرده‌اید؟{' '}
                        <button onClick={() => navigate('/Login')} className="cursor-pointer font-semibold text-indigo-400 hover:text-indigo-300">
                            ورود به حساب کاربری
                        </button>
                    </p>
                </div>
            </div>
        </>
    );
};

export default Register;
