import { combineReducers } from '@reduxjs/toolkit';
import fuse from './fuse';
import i18n from './i18nSlice';
import authorJoin from './authorJoinSlice';
import editorJoin from './editorJoinSlice';
import author from './authorSlice';
import authorWithoutAccount from './authorWithoutAccountSlice';
import editor from './editorSlice';
import country from './countrySlice';
import auth from './authSlice';
import book from './bookSlice';
import language from './languageSlice';
import category from './categorySlice';
import prize from './prizeSlice';
import subscriber from './subscriberSlice';
import bill from './billsSlice';
import promotion from './promotionSlice';
import communities from './communitySlice'
const createReducer = asyncReducers => (state, action) => {
	const combinedReducer = combineReducers({
		auth,
		fuse,
		authorJoin,
		editorJoin,
		author,
		authorWithoutAccount,
		editor,
		communities,
		country,
		book,
		language,
		category,
		prize,
		subscriber,
		bill,
		promotion,
		i18n,
		...asyncReducers
	});

	/*
	Reset the redux store when user logged out
	 */
	if (action.type === 'auth/user/userLoggedOut') {
		state = undefined;
	}

	return combinedReducer(state, action);
};

export default createReducer;
