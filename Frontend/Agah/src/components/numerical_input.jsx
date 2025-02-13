import { useState } from "react";

export default function NumericalInput({ inputName, label }) {
    const [value, setValue] = useState("");

    // Function to format the number with commas every 3 digits
    const formatNumber = (input) => {
        // Remove all non-digit characters
        const cleanedInput = input.replace(/\D/g, "");
        // Add commas every 3 digits
        return cleanedInput.replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    };

    // Handle input change
    const handleInputChange = (e) => {
        const formattedValue = formatNumber(e.target.value);
        setValue(formattedValue);
    };

    return (
        <div className="w-full mx-1 bg-white border border-gray-200 rounded-lg shadow-sm dark:bg-gray-800 dark:border-gray-700">
            <label htmlFor={inputName} className="sr-only">
                {label}
            </label>
            <input
                id={inputName}
                name={inputName}
                type="text" // Changed to "text" to allow commas
                className="block w-full p-2 py-3 text-sm text-gray-900 border border-gray-300 rounded-lg bg-white dark:bg-gray-800 dark:border-gray-700 focus:ring-blue-500 focus:border-blue-500 dark:text-white"
                value={value}
                onChange={handleInputChange}
                placeholder={label}
            />
        </div>
    );
}