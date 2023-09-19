import React, { useState } from 'react';

const UserRow = ({ index, validate, showDetails, cancel, rejected, validated }) => {
	const [status, setStatus] = useState('pending');
	return (
		<div style={{ display: 'flex', alignItems: 'center' }}>

			<span
				className="material-icons notranslate MuiIcon-root MuiIcon-fontSizeMedium text-orange-800 text-20 muiltr-1cpc5a8"
				aria-hidden="true"
				onClick={showDetails}
				title="afficher"
				aria-disabled={true}
			>
				info
				</span>

			{
				validate === true ?
					<span
						className="material-icons notranslate MuiIcon-root MuiIcon-fontSizeMedium text-gray text-20 muiltr-1cpc5a8"
						aria-hidden="true"
						onClick={validate}
						title="valider"
					>
						check_circle
				</span>
					:
					<span
						className="material-icons notranslate MuiIcon-root MuiIcon-fontSizeMedium text-green text-20 muiltr-1cpc5a8"
						aria-hidden="true"
						onClick={validate}
						title="valider"
					>
						check_circle
				</span>
			}


			<span
				className="material-icons notranslate MuiIcon-root MuiIcon-fontSizeMedium text-red text-20 muiltr-1cpc5a8"
				aria-hidden="true"
				onClick={cancel}
				title="rejeter"
			>
				clear_circle
				</span>
		
		</div>
	);
};

export default UserRow;
