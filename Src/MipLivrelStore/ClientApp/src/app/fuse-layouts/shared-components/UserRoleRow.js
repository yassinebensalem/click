import React, { useState } from 'react';

const UserRole = ({ remove, update, sendmail, confirm }) => {
	const [switchChecked, setSwitchChecked] = useState(true);

	return (
		<div style={{ display: 'flex', alignItems: 'center' }}>
			<span
				className="material-icons 
				notranslate 
				MuiIcon-root 
				MuiIcon-fontSizeMedium 
				text-blue-800 
				text-18 
				ml-10
				muiltr-1cpc5a8"
				aria-hidden="true"
				onClick={update}
				title="modifier"
			>
				edit
			</span>
			<span
				className="material-icons 
				notranslate 
				MuiIcon-root 
				MuiIcon-fontSizeMedium 
				text-red-800 
				text-18 
				
				muiltr-1cpc5a8"
				aria-hidden="true"
				onClick={remove}
				title="supprimer"
			>
				delete_outline
			</span>
     {
		confirm === true ?
		<span
				className="material-icons notranslate MuiIcon-root MuiIcon-fontSizeMedium text-gray text-20 muiltr-1cpc5a8"
				aria-hidden="true"
			>
				mail
				</span>
				:
			<span
				className="material-icons notranslate MuiIcon-root MuiIcon-fontSizeMedium text-red text-20 muiltr-1cpc5a8"
				aria-hidden="true"
				onClick={sendmail}
				title="e-mail de confirmation"
			>
				mail
				</span>
	 }
			
		</div>
	);
};

export default UserRole;
