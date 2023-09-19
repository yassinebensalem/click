import React from 'react';
import FuseCountdown from '@fuse/core/FuseCountdown';
const Packs = () => {
	return (
		<div className="flex justify-center items-center bg-stone-200 h-full">
			<div className="flex-col">
				<h2 className="text-center">Coming Soon...</h2>
				<FuseCountdown endDate="2022-02-1" className="my-48 justify-center" />
			</div>
		</div>
	);
};

export default Packs;
