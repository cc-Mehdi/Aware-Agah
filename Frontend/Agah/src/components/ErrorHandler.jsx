import { useSelector, useDispatch } from "react-redux";
import { clearError } from "../redux/errorSlice";

const ErrorHandler = () => {
    const error = useSelector((state) => state.error.message);
    const dispatch = useDispatch();

    if (!error) return null; // اگر خطایی نیست، چیزی نمایش نده

    return (
        <div className="fixed top-0 w-full left-0 z-50 ">


            <div className="grid h-screen place-content-center bg-white px-4 dark:bg-gray-900 opacity-90">
                <div className="text-center">
                    <h1 className="text-9xl font-black text-gray-200 dark:text-gray-700 mb-3">
                        <i className="fa-solid fa-triangle-exclamation"></i>
                    </h1>

                    <p className="text-2xl font-bold tracking-tight text-gray-900 sm:text-4xl dark:text-white">
                        همه چیز تحت کنترل است!
                    </p>

                    <p className="mt-4 text-gray-500 dark:text-gray-400">
                        متأسفیم، مشکلی در سیستم رخ داده است. لطفاً دوباره تلاش کنید یا با پشتیبانی تماس بگیرید.
                    </p>

                    <button
                        className="mt-6 cursor-pointer inline-block rounded-sm bg-indigo-600 px-5 py-3 text-sm font-medium text-white hover:bg-indigo-700 focus:ring-3 focus:outline-hidden"
                        onClick={() => dispatch(clearError())}
                    >
                        متوجه شدم
                    </button>
                </div>
            </div>



        </div>
    );
};

export default ErrorHandler;
