import { createAsyncThunk, createSlice, current } from '@reduxjs/toolkit';

import http from '../http-commons';

const initialState = {
	user: '',
	loading: null,
	isAuth: false,
	token: localStorage.getItem('token'),
	refreshToken: localStorage.getItem('refresh'),
	redirect: false,
	checked: false
};
export const login = createAsyncThunk('auth/login', async (form, { rejectWithValue }) => {
	try {
		const res = await http.post('/account/login', form);
		return res.data.data;
	} catch (err) {
		return rejectWithValue(err.response.data);
	}
});
export const getCurrent = createAsyncThunk('auth/current', async () => {
	try {
		const res = await http.get('/account/current');
        
		return res.data.data;
	} catch (err) {
	
	}
});

export const getRefresh = createAsyncThunk('auth/refresh', async (form) => {
	try {
		const res = await http.post('/account/refresh', form);
		return res.data;
	} catch (error) {
		
	}
})


const authSlice = createSlice({
	name: 'auth',
	initialState,
	reducers: {
		logout: state => {
			state.isAuth = false;
			state.user = '';
			state.token = null;
			state.refreshToken = null;
			state.firstName= '';
			state.lastName = '';
		},
		getAuthMe: state => {
			if (state.user === '') {
				state.isAuth = false;
			}else {
				state.isAuth = true;
			}
		},
		rememberMe: (state, {payload}) => {
			state.checked = payload;
		}

	},
	extraReducers: {
		[login.pending]: state => {
			state.loading = true;
			state.isAuth = false;
		},
		[login.fulfilled]: (state, { payload }) => {
			state.isAuth = true;
			state.loading = false;
			state.token = payload.accessToken;
			state.refreshToken = payload.refreshToken;
			localStorage.setItem('token', payload.accessToken);
			localStorage.setItem('refresh', payload.refreshToken);

		},
		[login.rejected]: state => {
			state.isAuth = false;
			state.loading = false;
		},
		[getCurrent.fulfilled]: (state, { payload }) => {
			if (payload === undefined) {
				state.user = '';
				state.isAuth = false;
				state.token = null;
				state.firstName='';
				state.lastName = '';
				localStorage.removeItem('token');
			}else {
				state.user = payload.claimsIdentity[0].value;
				state.firstName = payload.firstName;
				state.lastName = payload.lastName;
				state.isAuth = true;
				state.loading = false;
			}
			
		},
		[getCurrent.rejected]: (state, { payload }) => {
			state.loading = false;
		},
		[getRefresh.fulfilled]: (state, {payload}) => {	
	        state.refreshToken = payload && payload.data.refreshToken;
			// if(!state.refreshToken){
			// 	localStorage.removeItem('token')
			// 	state.token = null;
			// }
			localStorage.setItem('refresh', state.refreshToken);
		},
		[getRefresh.rejected]: (state) => {
			localStorage.removeItem('token')
		}
	}
});

export const { logout, getAuthMe, rememberMe } = authSlice.actions;

export default authSlice.reducer;