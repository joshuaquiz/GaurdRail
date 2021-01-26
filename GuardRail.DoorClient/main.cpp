/*
	Title --- 16keys2.cpp [examples]

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


#include <iostream>
#include <memory>

// Include Rpi-hw headers
#include "include/rpi-hw.hpp"
#include "include/rpi-hw/time.hpp"
#include "include/rpi-hw/keypad/matrix.hpp"

// Use Rpi-hw namespace
using namespace rpihw;

/*
	  (14, 15, 18, 23)   colums = 4
			||||
   ----------------------
   | (1)  (2)  (3)  (A) |
   |                    |
   | (4)  (5)  (6)  (B) |
   |                    |
   | (7)  (8)  (9)  (C) |
   |                    |
   | (*)  (0)  (#)  (D) |
   ----------------------
			||||
	   (24, 25, 8, 7)  rows = 4
*/

/** The class of my application **/
class MyApp {

public:

	// Define the keymap
	std::vector< uint8_t > keymap = {

		'1', '2', '3', 'A',
		'4', '5', '6', 'B',
		'7', '8', '9', 'C',
		'*', '0', '#', 'D'
	};

	/** Constructor method **/
	MyApp() : m_keypad(new keypad::matrix({ 7, 8, 25, 24 }, { 21, 20, 16, 12 }, keymap)) {

		keypad::T_EventListener listener = std::bind(&MyApp::eventListener, this, std::placeholders::_1);

		// Add the keypad event listener
		m_keypad->addEventListener(listener);
	}

	/** Destructor method **/
	~MyApp() = default;

    /** A simple keypad event listener **/
	void eventListener(keypad::keypad_base& dev) {

		const auto& keystate = dev.keyState();

		for (auto c : keystate)
		{
			std::cout << static_cast<char>(c) << std::flush;
		}

		std::cout << std::endl;
	}

	/** Main loop **/
	void run() {

		while (true)
		{

			/* ... */
		}
	}

private:

	// The keypad instance.
	std::unique_ptr< keypad::matrix > m_keypad;
};


int
main(int argc, char* args[]) {

	MyApp app;

	app.run();

	return 0;
}
