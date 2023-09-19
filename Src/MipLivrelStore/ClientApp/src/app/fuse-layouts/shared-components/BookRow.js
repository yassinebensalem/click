import React from 'react';

const UserRowActions = ({ validate, notPublish, cancel, status, edit, remove }) => {
	return (
		<div style={{ display: 'flex', alignItems: 'center' }}>
		<span
				className="material-icons 
				notranslate 
				MuiIcon-root 
				MuiIcon-fontSizeMedium 
				text-red-800 
				text-20 
			    mr-8
				muiltr-1cpc5a8"
				aria-hidden="true"
				onClick={remove}
				title="supprimer"
			>
				delete_outline
			</span>

				<span
					className="material-icons notranslate MuiIcon-root MuiIcon-fontSizeMedium text-green text-20 muiltr-1cpc5a8 mr-8"
					aria-hidden="true"
					onClick={validate}
					title="publier"
				>
					check_circle
				</span>
		

		
				<span
					className="material-icons notranslate MuiIcon-root MuiIcon-fontSizeMedium text-black text-20 muiltr-1cpc5a8 mr-8"
					aria-hidden="true"
					title="ne pas publier"
					style={{ width: '20px' }}
					onClick={notPublish}
				>
					unpublished_circle
				</span>
		
				<span
					className="material-icons notranslate MuiIcon-root MuiIcon-fontSizeMedium text-orange-800 text-20 muiltr-1cpc5a8 mr-8"
					aria-hidden="true"
					title="modifier"
					aria-disabled={true}
					onClick={edit}
				>
					edit
				</span>
		


			
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

export default UserRowActions;
