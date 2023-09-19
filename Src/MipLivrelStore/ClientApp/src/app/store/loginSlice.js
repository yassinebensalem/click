const { createSlice } = require('@reduxjs/toolkit');

const loginSlice = createSlice({
	name: 'auth',
	initialState: {
		isAuth: false
	},
	reducers: {
		login: (state, { payload }) => {
			state.isAuth = true;
		},
		logout: (state, { payload }) => {
			state.isAuth = false;
		}
	}
});

export const { login } = loginSlice.actions;

export default loginSlice.reducer;
