from Tencu_Framework_Server.models.Version import Version
from Tencu_Framework_Server.serializers.Version import VersionSerializer


def serialize_object_list(lst, serializer):
    if len(lst) == 0:
        return []
    return serializer(lst, many=True).data


def get_version(data: dict):
    version_obj = Version.objects.filter(name=data['name'])
    if len(version_obj) == 0:
        return {'name': data['name'], 'version': '0'}
    else:
        return VersionSerializer(version_obj[0]).data


def get_version_obj(name: str):
    version_obj = Version.objects.filter(name=name)
    if len(version_obj) == 0:
        version_obj = Version.objects.create(name=name, version=0)
    else:
        version_obj = version_obj[0]
    version_obj.version += 1
    return version_obj


def generic_delete(base_class, objects_filter: dict, version_name: str):
    element = base_class.objects.filter(**objects_filter)
    version_obj = get_version_obj(name=version_name)
    if len(element) == 0:
        return {'error': 'not found'}
    element.delete()
    version_obj.save()
    return {'success': 'deleted'}


def generic_add_or_edit(element, serializer, data: dict, version_name: str):
    version_obj = get_version_obj(name=version_name)
    if len(element) == 0:
        element_serializer = serializer(data=data)
        if element_serializer.is_valid():
            element_serializer.save()
            version_obj.save()
            return element_serializer.data
        return element_serializer.errors
    element_serializer = serializer(element[0], data=data, partial=True)
    if element_serializer.is_valid():
        element_serializer.save()
        version_obj.save()
        return element_serializer.data
    return element_serializer.errors


def get_file_from_database(base_class, filter_parameters):
    element = base_class.objects.filter(**filter_parameters)
    if len(element) == 0:
        return None
    return element[0].file
