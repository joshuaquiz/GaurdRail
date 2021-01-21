#include <functional>
#include "../include/task.h"

task::task()
{
	is_finished_ = true;
	thread_ = nullptr;
	throw std::exception();
}

task::task(std::thread* thread)
	: thread_(thread) {
	is_finished_ = false;
	thread_ = thread;
}

void task::wait() const
{
	thread_->join();
}

task task::run(std::function<void()> func)
{
	std::thread t(func);
	return task(&t);
}