import React from 'react';
import { Dialog, DialogContent, DialogTitle, Snackbar } from '@material-ui/core';
import MuiAlert from '@material-ui/lab/Alert';
const Notification = ({ title, children, openPopup, setOpenPopup }) => {
	return (
		<Dialog open={openPopup}>
			<DialogTitle>
				<div>title goes here</div>
			</DialogTitle>
			<DialogContent>
				<div>{children}</div>
			</DialogContent>
		</Dialog>
	);
};

export default Notification;
