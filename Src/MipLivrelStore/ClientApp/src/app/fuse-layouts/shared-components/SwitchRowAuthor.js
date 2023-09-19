import React, { useState, useEffect } from 'react';
import { Typography } from '@material-ui/core';

import { getAuthors, updateAuthor } from 'app/store/authorSlice';
import { useDispatch } from 'react-redux';

import Switch from 'react-switch';
const SwitchRowAuthor = ({ handleClick, author, authorId, isActive }) => {
	const [switchChecked, setSwitchChecked] = useState(isActive);
	const [active, setActive] = useState(false);

	const dispatch = useDispatch();
	const formData = new FormData();

	const handleChange = checked => {
		handleClick();
		setSwitchChecked(checked);
		if (author) {
			setSwitchChecked(!switchChecked);
			formData.append('Id', authorId);
			formData.append('FirstName', author.firstName);
			formData.append('LastName', author.lastName);
			formData.append('BirthDate', author.birthdate);
			formData.append('Email', author.email);
			formData.append('PhoneNumber', author.phoneNumber);
			formData.append('CountryId', author.countryId);
			formData.append('isActive', !switchChecked);
		}
		dispatch(updateAuthor(formData));
	};
	return (
		<div className="flex flex-row items-center">
			<Switch
				height={16}
				width={70}
				checked={switchChecked}
				onChange={handleChange}
				onColor="#f75454"
				uncheckedIcon={
					<div
						style={{
							display: 'flex',
							justifyContent: 'center',
							alignItems: 'center',
							height: '100%',
							fontSize: 10,
							color: '#fff',
							paddingRight: 50
						}}
					>
						Désactivé
					</div>
				}
				checkedIcon={
					<div
						style={{
							display: 'flex',
							justifyContent: 'center',
							alignItems: 'center',
							height: '100%',
							fontSize: 10,
							marginLeft: 20,
							color: '#FFF'
						}}
					>
						Activé
					</div>
				}
			/>
		</div>
	);
};

export default SwitchRowAuthor;
