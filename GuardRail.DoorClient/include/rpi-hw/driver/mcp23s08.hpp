/* 
	Title --- driver/mcp23s08.hpp

	Copyright (C) 2013 Giacomo Trudu - wicker25[at]gmail[dot]com

	This file is part of Rpi-hw.

	Rpi-hw is free software: you can redistribute it and/or modify
	it under the terms of the GNU Lesser General Public License as published by
	the Free Software Foundation version 3 of the License.

	Rpi-hw is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
	GNU Lesser General Public License for more details.

	You should have received a copy of the GNU Lesser General Public License
	along with Rpi-hw. If not, see <http://www.gnu.org/licenses/>.
*/


#ifndef _RPI_HW_DRIVER_MCP23S17_HPP_
#define _RPI_HW_DRIVER_MCP23S17_HPP_

#include "../rpi.hpp"

#include "../consts.hpp"
#include "../types.hpp"
#include "../exception.hpp"
#include "../utils.hpp"

#include "spi.hpp"
#include "mcp23x08.hpp"

namespace rpihw { // Begin main namespace

namespace driver { // Begin drivers namespace

/*!
	@class mcp23s08
	@brief 8-bit I/O Expander with SPI.
*/
class mcp23s08 : public mcp23x08 {

public:

	/*!
		@brief Constructor method.
		@param[in] dev_path The device path.
		@param[in] dev_id The device identifier.
	*/
	mcp23s08( const std::string &dev_path, uint8_t dev_id );

	//! Destructor method.
	virtual ~mcp23s08();

private:

	//! Serial Peripheral Interface.
	driver::spi *m_spi;

	//! Device identifier.
	uint8_t m_dev_id;

	//! Data buffer used for SPI transmission.
	uint8_t m_buffer[3];

	//! Sends data to the device.
	virtual void send( uint8_t reg, uint8_t data );

	//! Receives data from the device.
	virtual uint8_t receive( uint8_t reg );
};

} // End of drivers namespace

} // End of main namespace


// Include inline methods 
#include "mcp23s08-inl.hpp"

#endif /* _RPI_HW_DRIVER_MCP23S17_HPP_ */
