export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

// Function to get the latest token
export const getToken = () => localStorage.getItem("token");
