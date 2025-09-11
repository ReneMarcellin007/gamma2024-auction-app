import axios from "axios";

export const initApi = (getToken) => {
    const api = axios.create({
        baseURL: `${import.meta.env.VITE_API_URL}${import.meta.env.VITE_BASE_URL}`,
        avatarURL: `${import.meta.env.VITE_AVATAR_URL}`,
        withCredentials: true,
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        timeout: 30000,
    });

    // Intercepteur pour les requêtes
    api.interceptors.request.use(
        (config) => {
            const token = getToken();
            if (token) {
                config.headers.Authorization = `Bearer ${token}`;
            }
            return config;
        },
        (error) => {
            return Promise.reject(error);
        }
    );

    api.interceptors.response.use(
        (response) => {
            return response;
        },
        (error) => {
            if (error.response && error.response.status === 400) {
                return Promise.reject(error);
            }
            return Promise.reject(error);
        }
    );

    return api;
};

export default { initApi };
