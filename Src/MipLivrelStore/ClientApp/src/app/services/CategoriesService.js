import http from '../http-commons';

const create = (data, options) => {
	return http.post('/category/Add', data);
};
const remove = (id, options) => {
	return http.delete(`/category/Delete?id=${id}`, options);
};

const update = (data, options) => {
	return http.put(`/category/Update`, data, options);
};
export default {
	create,
	remove,
	update
};
