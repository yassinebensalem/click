import Icon from '@material-ui/core/Icon';
import Tooltip from '@material-ui/core/Tooltip';
import clsx from 'clsx';
import React, { useState } from 'react';
import IconButton from '@material-ui/core/IconButton';
import ExitToAppIcon from '@material-ui/icons/ExitToApp';

import styled from 'styled-components';
const LOGOUTBTN = styled.a`
	cursor: pointer;
	text-descoration: none;
`;
const Logout = () => {
	return (
		<LOGOUTBTN>
			<ExitToAppIcon
				style={{
					color: '#2cabe3'
				}}
			/>
		</LOGOUTBTN>
	);
};

export default Logout;
