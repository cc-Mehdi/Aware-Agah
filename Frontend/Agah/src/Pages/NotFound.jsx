import React from 'react'

const NotFound = () => {
    return (
        <section className="bg-transparent min-h-screen flex items-center justify-center">
            <div className="py-8 px-4 mx-auto max-w-screen-xl lg:py-16 lg:px-6">
                <div className="mx-auto max-w-screen-sm text-center bg-white/50 dark:bg-gray-900/50 backdrop-blur-md rounded-xl p-8 shadow-lg">
                    {/* Animated 404 Text */}
                    <h1 className="mb-4 text-7xl tracking-tight font-extrabold lg:text-9xl text-transparent bg-clip-text bg-gradient-to-r from-blue-600 to-sky-500 dark:from-blue-500 dark:to-sky-400 animate-bounce">
                        404
                    </h1>

                    {/* Main Heading */}
                    <p className="mb-4 text-3xl tracking-tight font-bold text-gray-900 md:text-4xl dark:text-white">
                        صفحه مورد نظر یافت نشد!
                    </p>

                    {/* Description */}
                    <p className="mb-8 text-lg font-light text-gray-600 dark:text-gray-300">
                        متأسفیم، به نظر می‌رسد صفحه‌ای که به دنبال آن هستید وجود ندارد یا حذف شده است. اما نگران نباشید، می‌توانید از صفحه اصلی شروع کنید.
                    </p>

                    {/* Back to Home Button */}
                    <a
                        href="/"
                        className="inline-flex items-center justify-center px-6 py-3 text-sm font-semibold text-white bg-gradient-to-r from-blue-600 to-sky-500 rounded-lg hover:from-blue-700 hover:to-sky-600 focus:ring-4 focus:outline-none focus:ring-blue-300 dark:focus:ring-blue-800 transition-all duration-300 ease-in-out transform hover:scale-105 shadow-lg hover:shadow-xl"
                    >
                        <svg
                            className="w-5 h-5 ml-2"
                            fill="none"
                            stroke="currentColor"
                            viewBox="0 0 24 24"
                            xmlns="http://www.w3.org/2000/svg"
                        >
                            <path
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                strokeWidth={2}
                                d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6"
                            />
                        </svg>
                        <span>
                            بازگشت به صفحه اصلی
                        </span>
                    </a>
                </div>
            </div>
        </section>
    )
}

export default NotFound