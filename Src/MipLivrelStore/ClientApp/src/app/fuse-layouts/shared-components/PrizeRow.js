import React from 'react';

const PrizeRow = ({ remove, update }) => {
    return (
        <div style={{ display: 'flex', alignItems: 'center' }}>

            <span
                className="material-icons notranslate MuiIcon-root MuiIcon-fontSizeMedium text-red text-20 muiltr-1cpc5a8"
                aria-hidden="true"
                onClick={remove}
                title="supprimer"
            >
                delete
            </span>
            <span
                className="material-icons notranslate MuiIcon-root MuiIcon-fontSizeMedium text-blue-800 ml-20 muiltr-1cpc5a8"
                aria-hidden="true"
                onClick={update}
                title="modifier"
            >
                edit
			</span>


        </div>
    );

}

export default PrizeRow;