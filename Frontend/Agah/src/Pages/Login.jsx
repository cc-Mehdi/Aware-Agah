import Logo from './../assets/images/logos/Full color.png';
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { api_HandleLogin } from './../services/api_BaseAPICaller';
import Toastr from './../components/toastr';


const Login = () => {

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();
    const [toastr, setToastr] = useState(null); // State to manage Toastr message


    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            if (email === "" || password === "") {
                setToastr({ type: "error", title: 'اطلاعات را با دقت وارد کنید!' });
            }
            else {
                const data = await api_HandleLogin(email, password); // Use email and password
                if (data.statusCode === 200) {
                    localStorage.setItem('token', data.token);
                    setToastr({ type: "success", title: data.statusMessage || "عملیات با موفقیت انجام شد" });
                    navigate('/'); // Navigate to the home page
                }
                else {
                    setToastr({ type: "error", title: data.statusMessage });
                }
            }
        } catch (error) {
            setToastr({ type: "error", title: "عملیات با شکست مواجه شد\nخطا: " + error });
        }

        setTimeout(() => setToastr(null), 3000);
    };

    return (
        <>
            {/* Conditionally render Toastr */}
            {toastr && <Toastr toastrType={toastr.type} title={toastr.title} />}

            <div className="flex min-h-full w-full flex-1 flex-col justify-center px-6 py-12 lg:px-8 bg-gray-900">
                <div className="sm:mx-auto sm:w-full sm:max-w-sm">
                    <img
                        alt="Agah"
                        src={Logo}
                        className="mx-auto h-18 rounded-xl w-auto"
                    />
                    <h2 className="mt-10 text-center text-2xl/9 font-bold tracking-tight text-white">
                        ورود به حساب کاربری
                    </h2>
                </div>

                <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
                    <form action="#" method="POST" className="space-y-6">
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
                            <div className="flex items-center justify-between">
                                <label htmlFor="password" className="block text-sm/6 font-medium text-gray-300">
                                    رمز عبور
                                </label>
                            </div>
                            <div className="mt-2">
                                <input
                                    id="password"
                                    name="password"
                                    type="password"
                                    required
                                    autoComplete="current-password"
                                    className="block w-full rounded-md bg-gray-800 px-3 py-1.5 text-base text-white outline-1 -outline-offset-1 outline-gray-600 placeholder:text-gray-400 focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-500 sm:text-sm/6"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                />
                                <div className="text-sm">
                                    <a href="#" className="font-semibold text-indigo-400 hover:text-indigo-300">
                                        رمز عبور را فراموش کرده‌اید؟
                                    </a>
                                </div>
                            </div>
                        </div>

                        <div>
                            <button
                                onClick={handleSubmit}
                                type="submit"
                                className="cursor-pointer flex w-full justify-center rounded-md bg-indigo-500 px-3 py-1.5 text-sm/6 font-semibold text-white shadow-xs hover:bg-indigo-400 focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-500"
                            >
                                ورود
                            </button>
                        </div>
                    </form>

                    <p className="mt-10 text-center text-sm/6 text-gray-400">
                        هنوز عضو نیستید؟{' '}
                        <button onClick={() => navigate('/Register')} className="cursor-pointer font-semibold text-indigo-400 hover:text-indigo-300">
                            ثبت نام رایگان
                        </button>
                    </p>
                </div>
            </div >
        </>
    )
}

export default Login