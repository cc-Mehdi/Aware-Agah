import { useState } from "react";

export default function dropdown({ inputName, label, dataSource }) {
    const [selectedData, setSelectedData] = useState("");

    // Ensure `dataSource` is always an array
    const safeDataSource = Array.isArray(dataSource) && dataSource.length > 0
        ? dataSource
        : [{ id: 0, title: "مقداری وجود ندارد" }];

    return (
        <div className="bg-white border-gray-200 w-full rounded-lg border shadow-sm dark:bg-gray-800 dark:border-gray-700">
            <label htmlFor={inputName} className="sr-only">
                {label}
            </label>
            <select
                id={inputName}
                name={inputName}
                className="p-2 py-3 text-gray-900 border-gray-300 bg-white bg-black block w-full rounded-lg border text-sm focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-800 dark:border-gray-700 dark:border-gray-500 dark:text-white"
                value={selectedData}
                onChange={(e) => setSelectedData(e.target.value)}
            >
                <option value="" disabled>
                    {label}
                </option>
                {safeDataSource.map((data) => (
                    <option key={data.id} value={data.id}>
                        {data.persianName}
                    </option>
                ))}
            </select>
        </div>
    );
}
