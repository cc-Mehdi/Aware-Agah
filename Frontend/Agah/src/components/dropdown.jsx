import { useState } from "react";

export default function dropdown({ inputName, label, dataSource }) {
    const [selectedData, setSelectedData] = useState("");

    if (dataSource == null) {
        dataSource = [
            "مقداری وجود ندارد"
        ]
    }

    return (
        <div className="w-full bg-white border border-gray-200 rounded-lg shadow-sm dark:bg-gray-800 dark:border-gray-700">
            <label htmlFor="data-select" className="sr-only">
                {label}
            </label>
            <select
                id={inputName}
                name={inputName}
                className="block w-full p-2 text-sm text-gray-900 border border-gray-300 rounded-lg bg-white dark:bg-gray-800 dark:border-gray-700 focus:ring-blue-500 focus:border-blue-500 bg-black dark:border-gray-500 dark:text-white"
                value={selectedData}
                onChange={(e) => setSelectedData(e.target.value)}
            >
                <option value="" disabled>
                    {label}
                </option>
                {dataSource.map((data, index) => (
                    <option key={index} value={data}>
                        {data}
                    </option>
                ))}
            </select>
        </div>
    );
}
