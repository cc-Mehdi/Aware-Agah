import { API_BASE_URL, getToken } from "./../../config.js";

const fetchApi = async ({ url, method = 'GET', body = null, headers = {}, requiresAuth = true }) => {
    const requestHeaders = {
        'Accept': '*/*',
        'Content-Type': 'application/json',
        ...headers
    };

    if (requiresAuth) {
        requestHeaders.authorization = `Bearer ${getToken()}`;
    }

    const response = await fetch(`${API_BASE_URL}${url}`, {
        method,
        headers: requestHeaders,
        body: body ? JSON.stringify(body) : null
    });

    const data = await response.json();
    return data;
};


export const getAlarms = async () => {
    return fetchApi({
        url: `/Alarms/GetAlarms`,
    });
};

export const getNotifications = async ({ alarmEnglishName }) => {
    return fetchApi({
        url: `/Notification/GetNotifications/${alarmEnglishName}`,
    });
};

export const getProductNames = async () => {
    return fetchApi({
        url: `/Product/GetProducts`,
    });
};

export const getProductsLog = async () => {
    return fetchApi({
        url: `/Product/GetProductsLog`,
    });
};

export const getReserve = async () => {
    return fetchApi({
        url: `/Reserve/Reserve`,
    });
};

export const getUser = async () => {
    return fetchApi({
        url: `/Users/User`,
    });
};

export const putUser = async (userEmail, userNewFullname) => {
    return fetchApi({
        url: `/Users/User`,
        method: 'PUT',
        body: {
            "Email": userEmail,
            "newFullname": userNewFullname
        }
    });
};

export const readAllNotifications = async () => {
    return fetchApi({
        url: `/Notification/ReadAllNotifications`,
    });
};

export const setPriceRangeReservation = async ({ productId, alarmId, minPrice, maxPrice }) => {
    return fetchApi({
        url: `/Reserve/SetPriceRangeReservation`,
        method: 'POST',
        body: {
            productId,
            alarmId,
            minPrice,
            maxPrice,
        }
    });
};

export const api_HandleLogin = async (email, password) => {
    return fetchApi({
        url: `/auth/Login`,
        method: 'POST',
        body: { email, password }
    });
};

export const api_HandleRegister = async (email, password) => {
    return fetchApi({
        url: `/auth/Register`,
        method: 'POST',
        body: { email, password }
    });
};

export const api_SendEmailVerification = async () => {
    return fetchApi({
        url: `/auth/SendEmailVerification`,
        method: 'POST'
    });
};

export const api_VerifyEmail = async (token) => {
    return fetchApi({
        url: `/auth/VerifyEmail/${token}`,
        method: 'POST'
    });
};