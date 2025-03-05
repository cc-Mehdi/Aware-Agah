import { API_BASE_URL } from "./../../config.js";

export const validateToken = async () => {
    const token = localStorage.getItem("token");
    if (!token) return false;

    try {
        const response = await fetch(`${API_BASE_URL}/Auth/ValidateToken`, {
            headers: { Authorization: `Bearer ${token}` },
        });

        if (!response.ok) throw new Error("Invalid token");

        return true;
    } catch (error) {
        return false;
    }
};