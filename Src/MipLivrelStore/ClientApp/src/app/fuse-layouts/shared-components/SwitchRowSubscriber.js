import React, { useState, useEffect } from 'react';
import { Typography } from '@material-ui/core';

import { updateSubscriber } from 'app/store/subscriberSlice';
import { useDispatch } from 'react-redux';
import Switch from 'react-switch';
const SwitchRowSubscriber = ({ handleClick, subscriber, subscriberId, isActive }) => {


    const [switchChecked, setSwitchChecked] = useState(isActive);
    const [active, setActive] = useState(false);

    const dispatch = useDispatch();
    const formData = new FormData();
    const handleChange = (checked) => {
        setSwitchChecked(checked);
        handleClick();

        if (subscriber) {
            setSwitchChecked(!switchChecked);
            formData.append('Id', subscriberId);
            formData.append('FirstName', subscriber.firstName);
            formData.append('LastName', subscriber.lastName);
            formData.append('BirthDate', subscriber.birthdate);
            formData.append('Email', subscriber.email);
            formData.append('PhoneNumber', subscriber.phoneNumber);
            formData.append('CountryId', subscriber.countryId);
            formData.append('gender', subscriber.gender);
            formData.append('Password', undefined);
            formData.append('address', subscriber.address);
            formData.append('isActive', !switchChecked);
        }

        dispatch(updateSubscriber(formData));
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
                        paddingLeft: "-4px"                 
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

export default SwitchRowSubscriber;
