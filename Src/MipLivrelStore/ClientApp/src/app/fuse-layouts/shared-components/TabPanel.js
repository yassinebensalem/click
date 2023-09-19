import { Box, Typography } from '@material-ui/core';
import React from 'react';

const TabPanel = ({ children, value, index, ...other }) => {
	return (
		<Typography
			component="div"
			role="tabpanel"
			hidden={value !== index}
			id={`scrollable-auto-tabpanel-${index}`}
			aria-labelledby={`scrollable-auto-tab-${index}`}
			{...other}
		>
			<Box p={3}>{children}</Box>
		</Typography>
	);
};

export default TabPanel;
