import axios from 'axios';

let URL;

console.log('MIP environment : ' + process.env.NODE_ENV );
if (process.env.NODE_ENV === 'development') {
	URL = 'https://localhost:44367/api';
} else if (process.env.NODE_ENV === 'production') {
	URL = 'https://miplivrel.azurewebsites.net/api';
}




const axiosInstance = axios.create({
	baseURL: URL
});

axiosInstance.interceptors.request.use(
	(config) => {
		const token = localStorage.getItem('token');
		if (token) {
			config.headers.Authorization = `Bearer ${token}`;
		}
		return config;
	},
	(error) => Promise.reject(error),
);
export default axiosInstance;