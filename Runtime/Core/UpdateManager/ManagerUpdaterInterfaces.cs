/*
* Copyright (C) 2023
* by Szymon Miś
* All rights reserved;
*/

namespace SMF.Core
{
	public interface IFixedUpdateInvoker
	{
		void OnFixedUpdate(float fixedDeltaTime);
	}

	public interface IUpdateInvoker
	{
		void OnUpdate(float deltaTime);
	}

	public interface ILateUpdateInvoker
	{
		void OnLateUpdate(float deltaTime);
	}
}