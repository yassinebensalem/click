import React, { useState } from 'react';

const SubscriberRow = ({ remove, update }) => {
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

		</div>
	);
};

export default SubscriberRow;
