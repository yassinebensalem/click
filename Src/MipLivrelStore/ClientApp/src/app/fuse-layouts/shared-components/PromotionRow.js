import React, { useState } from 'react';

const PromotionRow = ({  update, deletePromotion}) => {
	const [status, setStatus] = useState('pending');
	return (
		<div style={{ display: 'flex', alignItems: 'center' }}>
			<span
				className="material-icons notranslate MuiIcon-root MuiIcon-fontSizeMedium text-blue-800 text-18 muiltr-1cpc5a8"
				aria-hidden="true"
				onClick={update}
				title="modifier"
			>
				edit
			</span>
			<span
				className="material-icons notranslate MuiIcon-root MuiIcon-fontSizeMedium text-red-800  ml-12 text-20 muiltr-1cpc5a8"
				aria-hidden="true"
				onClick={deletePromotion}
				title="supprimer"
			>
				delete
			</span>
		</div>
	);
};

export default PromotionRow;
