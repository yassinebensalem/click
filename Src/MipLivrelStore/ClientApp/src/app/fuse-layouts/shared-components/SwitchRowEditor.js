import React, { useState, useEffect } from 'react';
import {  Typography } from '@material-ui/core';

import { updateEditor } from 'app/store/editorSlice';
import { useDispatch } from 'react-redux';
import Switch from 'react-switch';
const SwitchRowEditor = ({ handleClick, editor, editorId, isActive }) => {


    const [switchChecked, setSwitchChecked] = useState(isActive);
    const [active, setActive] = useState(false);

    const dispatch = useDispatch();
    const formData = new FormData();
    const handleChange = (checked) => {
        setSwitchChecked(checked);
        handleClick();
        if (editor) {
            setSwitchChecked(!switchChecked);
            formData.append('Id', editorId);
            formData.append('RaisonSocial', editor.raisonSocial);
            formData.append('IdFiscal', editor.idFiscal);
            formData.append('BirthDate', editor.birthdate);
            formData.append('Email', editor.email);
            formData.append('PhoneNumber', editor.phoneNumber);
            formData.append('CountryId', editor.countryId);
            formData.append('isActive', !switchChecked);
            formData.append('RateOnOriginalPrice', editor.rateOnOriginalPrice.toString().replace('.',','));
		        formData.append('RateOnSale', editor.rateOnSale.toString().replace('.', ','));
        }
        dispatch(updateEditor(formData));
    }

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
                        display: "flex",
                        justifyContent: "center",
                        alignItems: "center",
                        height: "100%",
                        fontSize: 10,
                        marginRight: 70,
                        color: "#fff",
                        paddingLeft: "-4px",      
                        paddingRight:"4px"           
                      }}
                    >
                      Désactivé
                    </div>
                  }
                  checkedIcon={
                    <div
                      style={{
                        display: "flex",
                        justifyContent: "center",
                        alignItems: "center",
                        height: "100%",
                        fontSize: 10,
                        marginLeft: 20,
                        color: "#FFF",

                       
                      }}
                    >
                      Activé
                    </div>
                  }
      
            />
          
     
     
        </div>
    );
};

export default SwitchRowEditor;
