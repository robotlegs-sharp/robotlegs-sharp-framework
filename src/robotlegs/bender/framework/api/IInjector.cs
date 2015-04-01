//------------------------------------------------------------------------------
//  Copyright (c) 2014-2015 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------

﻿using System;
using swiftsuspenders;
using swiftsuspenders.dependencyproviders;
using swiftsuspenders.mapping;
using swiftsuspenders.typedescriptions;

namespace robotlegs.bender.framework.api
{
	public interface IInjector
	{
		event Injector.InjectionMappingDelegate MappingOverride;

		event Injector.InjectionDelegate PostConstruct;
		
		event Injector.InjectionDelegate PostInstantiate;
		
		event Injector.InjectionMappingDelegate PostMappingChange;
		
		event Injector.InjectionMappingDelegate PostMappingCreate;
		
		event Injector.MappingDelegate PostMappingRemove;
		
		event Injector.InjectionDelegate PreConstruct;
		
		event Injector.InjectionMappingDelegate PreMappingChange;
		
		event Injector.MappingDelegate PreMappingCreate;
		
		/**
		 * Sets the parent <code>IInjector</code>
		 * @param parentInjector The parent IInjector used for dependencies the
		 * current IInjector can't supply
		 */
		IInjector parent { get; set; }

		/**
		 * Sets the ApplicationDomain to use for type reflection
		 * @param applicationDomain The ApplicationDomain
		 */
//		function set applicationDomain(applicationDomain:ApplicationDomain):void;

		/**
		 * The ApplicationDomain used for type reflection
		 */
//		function get applicationDomain():ApplicationDomain;

		/**
		* The current FallbackDependencyProvider
		*/
		FallbackDependencyProvider fallbackProvider {get;set;}

		/**
		 * Disables parent FallbackProvider
		 * @param value True/false
		 */
		bool blockParentFallbackProvider { get; set; }

		/**
		 * Instructs the injector to use the description for the given type when constructing or
		 * destroying instances.
		 *
		 * The description consists details for the constructor, all properties and methods to
		 * inject into during construction and all methods to invoke during destruction.
		 *
		 * @param type
		 * @param description
		 */
		void AddTypeDescription (Type type, TypeDescription description);

		/**
		 * Returns a description of the given type containing its constructor, injection points
		 * and post construct and pre destroy hooks
		 *
		 * @param type The type to describe
		 * @return The TypeDescription containing all information the injector has about the type
		 */
		TypeDescription GetTypeDescription (Type type);

		/**
		 * Does this injector (or any parents) have a mapping for the given type?
		 * @param type The type
		 * @param name Optional name
		 * @return True if the mapping exists
		 */
		bool HasMapping <T>(object name = null);
		bool HasMapping (Type type, object name = null);

		/**
		 * Does this injector have a direct mapping for the given type?
		 * @param type The type
		 * @param name Optional name
		 * @return True if the mapping exists
		 */
		bool HasDirectMapping<T>(object name = null);
		bool HasDirectMapping(Type type, object name = null);

		/**
		 * Maps a request description, consisting of the <code>type</code> and, optionally, the
		 * <code>name</code>.
		 *
		 * <p>The returned mapping is created if it didn't exist yet or simply returned otherwise.</p>
		 *
		 * <p>Named mappings should be used as sparingly as possible as they increase the likelyhood
		 * of typing errors to cause hard to debug errors at runtime.</p>
		 *
		 * @param type The <code>class</code> describing the mapping
		 * @param name The name, as a case-sensitive string, to further describe the mapping
		 *
		 * @return The <code>InjectionMapping</code> for the given request description
		 *
		 * @see #unmap()
		 * @see org.swiftsuspenders.mapping.InjectionMapping
		 */
		InjectionMapping Map<T>(object name = null);
		InjectionMapping Map(Type type, object name = null);

		/**
		 *  Removes the mapping described by the given <code>type</code> and <code>name</code>.
		 *
		 * @param type The <code>class</code> describing the mapping
		 * @param name The name, as a case-sensitive string, to further describe the mapping
		 *
		 * @throws org.swiftsuspenders.errors.InjectorError Descriptions that are not mapped can't be unmapped
		 * @throws org.swiftsuspenders.errors.InjectorError Sealed mappings have to be unsealed before unmapping them
		 *
		 * @see #map()
		 * @see org.swiftsuspenders.mapping.InjectionMapping
		 * @see org.swiftsuspenders.mapping.InjectionMapping#unseal()
		 */
		void Unmap<T>(object name = null);
		void Unmap(Type type, object name = null);

		/**
		 * Indicates whether the injector can supply a response for the specified dependency either
		 * by using a mapping of its own or by querying one of its ancestor injectors.
		 *
		 * @param type The type of the dependency under query
		 * @param name The name of the dependency under query
		 *
		 * @return <code>true</code> if the dependency can be satisfied, <code>false</code> if not
		 */
		bool Satisfies<T>(object name = null);
		bool Satisfies(Type type, object name = null);

		/**
		 * Indicates whether the injector can directly supply a response for the specified
		 * dependency.
		 *
		 * <p>In contrast to <code>#satisfies()</code>, <code>satisfiesDirectly</code> only informs
		 * about mappings on this injector itself, without querying its ancestor injectors.</p>
		 *
		 * @param type The type of the dependency under query
		 * @param name The name of the dependency under query
		 *
		 * @return <code>true</code> if the dependency can be satisfied, <code>false</code> if not
		 */
		bool SatisfiesDirectly<T>(object name = null);
		bool SatisfiesDirectly(Type type, object name = null);

		/**
		 * Returns the mapping for the specified dependency class
		 *
		 * <p>Note that getMapping will only return mappings in exactly this injector, not ones
		 * mapped in an ancestor injector. To get mappings from ancestor injectors, query them
		 * using <code>parentInjector</code>.
		 * This restriction is in place to prevent accidential changing of mappings in ancestor
		 * injectors where only the child's response is meant to be altered.</p>
		 *
		 * @param type The type of the dependency to return the mapping for
		 * @param name The name of the dependency to return the mapping for
		 *
		 * @return The mapping for the specified dependency class
		 *
		 * @throws org.swiftsuspenders.errors.InjectorMissingMappingError when no mapping was found
		 * for the specified dependency
		 */
		InjectionMapping GetMapping<T>(object name = null);
		InjectionMapping GetMapping(Type type, object name = null);

		/**
		 * Inspects the given object and injects into all injection points configured for its class.
		 *
		 * @param target The instance to inject into
		 *
		 * @throws org.swiftsuspenders.errors.InjectorError The <code>Injector</code> must have mappings
		 * for all injection points
		 *
		 * @see #map()
		 */
		void InjectInto(object target);

		/**
		 * Instantiates the class identified by the given <code>type</code> and <code>name</code>.
		 *
		 * <p>The parameter <code>targetType</code> is only useful if the
		 * <code>InjectionMapping</code> used to satisfy the request might vary its result based on
		 * that <code>targetType</code>. An Example of that would be a provider returning a logger
		 * instance pre-configured for the instance it is used in.</p>
		 *
		 * @param type The <code>class</code> describing the mapping
		 * @param name The name, as a case-sensitive string, to use for mapping resolution
		 * @param targetType The type of the instance that is dependent on the returned value
		 *
		 * @return The mapped or created instance
		 *
		 * @throws org.swiftsuspenders.errors.InjectorMissingMappingError if no mapping was found
		 * for the specified dependency and no <code>fallbackProvider</code> is set.
		 */
		T GetInstance<T>(object name = null, Type targetType = null);
		object GetInstance(Type type, object name = null, Type targetType = null);

		/**
		 * Returns an instance of the given type. If the Injector has a mapping for the type, that
		 * is used for getting the instance. If not, a new instance of the class is created and
		 * injected into.
		 *
		 * @param type The type to get an instance of
		 * @return The instance that was created or retrieved from the mapped provider
		 *
		 * @throws org.swiftsuspenders.errors.InjectorMissingMappingError if no mapping is found
		 * for one of the type's dependencies and no <code>fallbackProvider</code> is set
		 * @throws org.swiftsuspenders.errors.InjectorInterfaceConstructionError if the given type
		 * is an interface and no mapping was found
		 */
		T GetOrCreateNewInstance<T>();
		object GetOrCreateNewInstance(Type type);

		/**
		 * Creates an instance of the given type and injects into it.
		 *
		 * @param type The type to instantiate
		 * @return The new instance, with all of its dependencies fulfilled
		 *
		 * @throws org.swiftsuspenders.errors.InjectorMissingMappingError if no mapping is found
		 * for one of the type's dependencies and no <code>fallbackProvider</code> is set
		 */
		T InstantiateUnmapped<T>();
		object InstantiateUnmapped(Type type);

		/**
		 * Uses the <code>TypeDescription</code> the injector associates with the given instance's
		 * type to iterate over all <code>[PreDestroy]</code> methods in the instance, supporting
		 * automated destruction.
		 *
		 * @param instance The instance to destroy
		 */
		void DestroyInstance(object instance);

		/**
		 * Destroys the injector by cleaning up all instances it manages.
		 *
		 * Cleanup in this context means iterating over all mapped dependency providers and invoking
		 * their <code>destroy</code> methods and calling preDestroy methods on all objects the
		 * injector created or injected into.
		 *
		 * Of note, the <link>SingletonProvider</link>'s implementation of <code>destroy</code>
		 * invokes all preDestroy methods on the managed singleton to guarantee its orderly
		 * destruction. Implementers of custom implementations of <link>DependencyProviders</link>
		 * are encouraged to do likewise.
		 */
		void Teardown();

		/**
		 * Creates a new <code>Injector</code> and sets itself as that new <code>Injector</code>'s
		 * <code>parentInjector</code>.
		 *
		 * @param applicationDomain The optional domain to use in the new Injector.
		 * If not given, the creating injector's domain is set on the new Injector as well.
		 * @return The newly created <code>Injector</code> instance
		 *
		 * @see #parent
		 */
		IInjector CreateChild(/*applicationDomain:ApplicationDomain = null*/);
	}
}

