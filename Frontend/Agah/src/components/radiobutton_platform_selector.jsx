import React from "react";
import Skeleton from "./skeleton.jsx";

const RadioButtonPlatformSelector = ({ inputName, dataSource, selectedValue, onChange }) => {

    if (dataSource === 'no_data') {
        return (
            <Skeleton />
        )
    }

    return (
        <ul className="grid w-full gap-6 md:grid-cols-2">
            {dataSource.map((platform) => (
                <li key={platform.id}>
                    <input
                        type="radio"
                        id={platform.id}
                        name={inputName} // Ensures all radios belong to the same group
                        value={platform.id}
                        className="hidden peer"
                        checked={selectedValue === platform.id.toString()} // Controls the selection
                        onChange={(e) => onChange(e.target.value)} // Updates the parent state
                        required
                    />
                    <label
                        htmlFor={platform.id}
                        className="inline-flex items-center justify-between w-full p-5 text-gray-500 bg-white border border-gray-200 rounded-lg cursor-pointer dark:hover:text-gray-300 dark:border-gray-700 dark:peer-checked:text-blue-500 peer-checked:border-blue-600 dark:peer-checked:border-blue-600 peer-checked:text-blue-600 hover:text-gray-600 hover:bg-gray-100 dark:text-gray-400 dark:bg-gray-800 dark:hover:bg-gray-700"
                    >
                        <div className="block">
                            <div className="w-full text-lg font-semibold">{platform.persianName}</div>
                            <div className="w-full">{platform.shortDescription}</div>
                        </div>
                        <svg
                            className="w-5 h-5 ms-3 rtl:rotate-180"
                            aria-hidden="true"
                            xmlns="http://www.w3.org/2000/svg"
                            fill="none"
                            viewBox="0 0 14 10"
                        >
                            <path
                                stroke="currentColor"
                                strokeLinecap="round"
                                strokeLinejoin="round"
                                strokeWidth="2"
                                d="M1 5h12m0 0L9 1m4 4L9 9"
                            />
                        </svg>
                    </label>
                </li>
            ))}
        </ul>
    );
};

export default RadioButtonPlatformSelector;
